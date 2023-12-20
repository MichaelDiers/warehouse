import { createSlice } from '@reduxjs/toolkit'
import type { PayloadAction } from '@reduxjs/toolkit'
import IFrontendStockItem from '../types/frontend-stock-item'

export interface ISelectedStockItemState {
  current?: IFrontendStockItem
}

const initialState: ISelectedStockItemState = {
}

export const selectedStockItemSlice = createSlice({
  name: 'selectedStockItem',
  initialState,
  reducers: {
    setSelectedStockItem: (state, action: PayloadAction<IFrontendStockItem>) => {
      state.current = action.payload;
    },
    resetSelectedStockItem: (state) => {
      state.current = undefined;
    },
  },
})

export const { setSelectedStockItem, resetSelectedStockItem } = selectedStockItemSlice.actions

export default selectedStockItemSlice.reducer