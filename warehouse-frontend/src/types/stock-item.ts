import ILink from './link';

export default interface IStockItem {
  links: ILink[];
  minimumQuantity: number;
  name: string;
  quantity: number;
}
