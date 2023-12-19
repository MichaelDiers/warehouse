import { Link } from 'react-router-dom';
import IText from "../../text/text";
import AppRoutes from '../../types/app-routes.enum';
import { useGetStockItemsQuery } from './stock-item-list-api-slice';
import StockItem from '../../components/form-elements/stock-item/StockItem';

export function StockItemList({
  text
}: {
  text: IText
}) {
  const { data: stockItems } = useGetStockItemsQuery();

  return (
    <>
      <h1>{text.stockItemListHeader}</h1>
      <ul>
        {stockItems?.map((stockItem, index) => (
          <li key={index}>
            <StockItem
              detailsUrl={`${AppRoutes.STOCK_ITEM_DETAILS}?url=${stockItem.detailsUrl}`}
              minimumQuantity={stockItem.minimumQuantity}
              text={text}
              quantity={stockItem.quantity}
              name={stockItem.name}
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
