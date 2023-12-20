import IText from '../../text/text'
import StockItem from '../../components/form-elements/stock-item/StockItem';
import { useSelector } from 'react-redux';
import { RootState } from '../../app/store';

export function StockItemDetails({
  text
}: {
  text: IText
}) {
  const stockItem = useSelector((state) => (state as RootState).selectedStockItem.current);

  return (
    <StockItem
      headlineText={text.stockItemDetailsHeader}
      stockItem={stockItem}
      text={text}
      type='details'
    />
  )
}
