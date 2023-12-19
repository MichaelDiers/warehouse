import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import type {
  BaseQueryApi,
  BaseQueryFn,
  FetchArgs,
  FetchBaseQueryError,
} from '@reduxjs/toolkit/query/react'
import Urn from '../types/urn.enum';
import { RootState } from './store';
import { append } from './options-slice';
import ApiTagTypes from '../types/api-tag-types';

interface IData {
  links: ILink[];
}
interface ILink {
  url: string;
  urn: string;
}

const optionsQuery = fetchBaseQuery({
  baseUrl: 'http://localhost:5008',
  method: 'OPTIONS',
  prepareHeaders: (headers, { getState }) => {
    headers.set('Content-Type', 'application/json');
    headers.set('x-api-key', 'The api key');
    headers.set('Authorization', `Bearer ${(getState() as RootState).user.current?.accessToken || ''}`);
    return headers;
  }
})

const rawBaseQuery = fetchBaseQuery({
  baseUrl: 'http://localhost:5008',
  prepareHeaders: (headers, { getState }) => {
    headers.set('Content-Type', 'application/json');
    headers.set('x-api-key', 'The api key');
    headers.set('Authorization', `Bearer ${(getState() as RootState).user.current?.accessToken || ''}`);
    return headers;
  },
})

const getUrlForUrnFromCache = (urn: Urn, api: BaseQueryApi): string | undefined => {
  const state = api.getState() as RootState;
  const links = state.options.links;
  const link = links.find((link: ILink) => link.urn === urn);
  return link?.url;
}

const getUrlForUrnFromRequest = async (url: string, urn: Urn, api: BaseQueryApi): Promise<string> => {
  const options = await optionsQuery({ url }, api, {});
  const linkData: IData = options.data as IData;

  if (options.error || !linkData.links) {
    console.error(options.error);
    throw new Error(`Cannot read from ${url}`);
  }

  api.dispatch(append(linkData.links));

  const option = linkData.links.find((link: ILink) => link.urn === urn);
  if (!option || !option.url) {
    throw new Error(`Cannot find url for urn ${urn}`);
  }

  return option.url;
}

const getUrlForUrn = async (url: string, urn: Urn, api: BaseQueryApi): Promise<string> => {
  const cachedUrl = getUrlForUrnFromCache(urn, api);
  if (cachedUrl) {
    return cachedUrl;
  }

  return getUrlForUrnFromRequest(url, urn, api);
}

const getUrl = async (urn: Urn, api: BaseQueryApi): Promise<string> => {
  const optionsUrn = `urn:${urn.split(':')[1]}:Options`;
  const optionsUrl = await getUrlForUrn('/api/Options', optionsUrn as Urn, api);

  return getUrlForUrn(optionsUrl, urn, api);
}

const dynamicBaseQuery: BaseQueryFn<
  string | FetchArgs,
  unknown,
  FetchBaseQueryError
> = async (args: any, api, extraOptions) => {
  const providedUrl: string = args['url'];
  const url = providedUrl.toLocaleLowerCase().startsWith('urn:') ? await getUrl(providedUrl as Urn, api) : providedUrl;

  if (!url) {
    throw new Error('empty url');
  }

  const adjustedArgs = { ...args, url }

  return rawBaseQuery(adjustedArgs, api, extraOptions)
}

/**
 * The base api slice for service calls. Each feature injects its endpoints
 * and enhance the api slice by new tags.
 */
export const apiSlice = createApi({
  baseQuery: dynamicBaseQuery,
  endpoints: () => ({}),
  tagTypes: [ApiTagTypes.STOCK_ITEM]
});

export default apiSlice;
