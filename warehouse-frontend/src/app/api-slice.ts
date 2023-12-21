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
import { updateUserThunk } from './user-slice';

interface IData {
  links: ILink[];
}

interface ITokenResponse {
  accessToken: string;
  refreshToken: string;
}

const baseQuery: BaseQueryFn<
  string | FetchArgs,
  unknown,
  FetchBaseQueryError
> = async (args: string|FetchArgs, api: BaseQueryApi, extraOptions: any) => {
  const state = api.getState() as RootState;

  const query = fetchBaseQuery({
    baseUrl: state.baseUrl.current,
    prepareHeaders: (headers, { getState }) => {
      const state = getState() as RootState;

      headers.set('Content-Type', 'application/json');
      headers.set('x-api-key', state.apiKey.current);
      
      const token = extraOptions?.token;
      !!token && headers.set('Authorization', `Bearer ${token}`);
      
      return headers;
    }
  });
  
  return query(args, api, extraOptions);
}

const baseQueryReauth: BaseQueryFn<
  FetchArgs,
  unknown,
  FetchBaseQueryError
> = async (args: FetchArgs, api: BaseQueryApi, extraOptions: any) => {
  const providedUrl: string = args.url;
  const url = providedUrl.toLocaleLowerCase().startsWith('urn:')
    ? await getUrl(providedUrl as Urn, api)
    : providedUrl;

  if (!url) {
    throw new ApplicationError(selectText(api).cannotFindUrlError(providedUrl));
  }

  const state = api.getState() as RootState;

  const adjustedArgs = { ...args, url }

  const response = await baseQuery(
    adjustedArgs,
    api,
    {...extraOptions, token: state.user.current?.accessToken},
  );

  if (
    !response.error 
    || response.error.status !== 401 
    || !state.user.current?.refreshToken) {
    return response;
  }

  let refreshUrl;
  try {
    refreshUrl = await getUrl(Urn.AUTH_REFRESH, api);
  } catch {
    return response;
  }

  const tokenResponse = await baseQuery({
    url: refreshUrl,
    method: 'POST'
  },
  api,
  {
    token: state.user.current.refreshToken
  });

  if (tokenResponse.error) {
    return response;
  }

  const { accessToken, refreshToken } = tokenResponse.data as ITokenResponse;
  api.dispatch(updateUserThunk(accessToken, refreshToken));
  return baseQuery(
    adjustedArgs,
    api,
    {
      ...extraOptions,
      token: accessToken,
    },
  );
}

const getUrlForUrnFromCache = (urn: Urn, api: BaseQueryApi): string | undefined => {
  const state = api.getState() as RootState;
  const links = state.options.links;
  const link = links.find((link: ILink) => link.urn === urn);
  return link?.url;
}

const getUrlForUrnFromRequest = async (url: string, urn: Urn, api: BaseQueryApi): Promise<string> => {
  const options = await baseQueryReauth({ url, method: 'OPTIONS' }, api, {});
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

/**
 * The base api slice for service calls. Each feature injects its endpoints
 * and enhance the api slice by new tags.
 */
export const apiSlice = createApi({
  baseQuery: baseQueryReauth,
  endpoints: () => ({}),
  tagTypes: [ApiTagTypes.STOCK_ITEM]
});

export default apiSlice;
