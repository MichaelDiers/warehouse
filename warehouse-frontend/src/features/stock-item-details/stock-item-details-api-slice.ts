import apiSplice from '../../app/api-slice';
import ApiTagTypes from '../../types/api-tag-types';
import IFrontendStockItem from '../../types/frontend-stock-item';
import ILink from '../../types/link';
import IStockItem from '../../types/stock-item';
import Urn from '../../types/urn.enum';

const stockItemDetailsApiSlice = apiSplice.injectEndpoints({
  endpoints: builder => ({
    getStockItem: builder.query<IFrontendStockItem, string>({
      query: (url: string) => ({
        url,
        method: 'GET',
      }),
      providesTags: [ApiTagTypes.STOCK_ITEM],
      transformResponse: (response: IFrontendStockItem) => {
        const backendStockItem = response as IStockItem;
        const detailsUrl = backendStockItem.links.find((link: ILink) => link.urn === Urn.STOCK_ITEM_READ_BY_ID)?.url;
        return { ...response, detailsUrl };
      }
    }),
  }),
  overrideExisting: false,
});

export const {
  useGetStockItemQuery,
} = stockItemDetailsApiSlice;
