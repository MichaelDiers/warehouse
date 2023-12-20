import { Link } from 'react-router-dom';
import IText from "../../text/text";
import AppRoutes from '../../types/app-routes.enum';
import { useGetStockItemsQuery } from './stock-item-list-api-slice';
import StockItem from '../../components/form-elements/stock-item/StockItem';
import { QueryStatus } from '@reduxjs/toolkit/dist/query';

export function StockItemList({
  text
}: {
  text: IText
}) {
  const { data: stockItems, status } = useGetStockItemsQuery();

  return (
    <>
      <h1>{text.stockItemListHeader}</h1>
      <ul>
        {stockItems?.map((stockItem, index) => (
          <li key={index}>
            <StockItem
              isInProgress={status === QueryStatus.pending}
              stockItem={stockItem}
              text={text}
              type='list'
            />
          </li>
        ))}
      </ul>
      <Link to={AppRoutes.STOCK_ITEM_CREATE}>
        {text.stockItemCreateLinkLabel}
      </Link>
    </>
  )
}
