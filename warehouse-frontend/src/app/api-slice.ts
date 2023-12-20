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
import ILink from '../types/link';
import ApplicationError from '../types/application-error';
import selectText from '../text/text-selector';

interface IData {
  links: ILink[];
}

const defaultQuery = ({
  method
}: {
  method?: string
}) => fetchBaseQuery({
  baseUrl: 'http://localhost:5008',
  method,
  prepareHeaders: (headers, { getState }) => {
    headers.set('Content-Type', 'application/json');
    headers.set('x-api-key', 'The api key');
    const token = (getState() as RootState).user.current?.accessToken;
    if (token) {
      headers.set('Authorization', `Bearer ${token}`);
    }

    return headers;
  }
});

const optionsQuery = defaultQuery({ method: 'OPTIONS' });

const rawBaseQuery = defaultQuery({});

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
    throw new ApplicationError(selectText(api).optionsError);
  }

  api.dispatch(append(linkData.links));

  const option = linkData.links.find((link: ILink) => link.urn === urn);
  if (!option || !option.url) {
    throw new ApplicationError(selectText(api).cannotFindUrlForUrnError(urn));
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
  const url = providedUrl.toLocaleLowerCase().startsWith('urn:')
    ? await getUrl(providedUrl as Urn, api)
    : providedUrl;

  if (!url) {
    throw new ApplicationError(selectText(api).cannotFindUrlError(providedUrl));
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
