import { Link } from 'react-router-dom';
import IText from "../../text/text";
import AppRoutes from '../../types/app-routes.enum';
import { useGetStockItemsQuery } from './stock-item-list-api-slice';
import StockItemName from '../../components/form-elements/stock-item/StockItemName';
import Quantity from '../../components/form-elements/stock-item/Quantity';
import MinimumQuantity from '../../components/form-elements/stock-item/MinimumQuantity';

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
            <StockItemName value={stockItem.name} text={text} />
            <Quantity value={stockItem.quantity.toString()} text={text} />
            <MinimumQuantity value={stockItem.minimumQuantity.toString()} text={text} />
          </li>
        ))}
      </ul>
      <Link to={AppRoutes.STOCK_ITEM_CREATE}>
        {text.stockItemCreateLinkLabel}
      </Link>
    </>
  )
}
