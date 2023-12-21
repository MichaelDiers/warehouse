import { createSlice } from '@reduxjs/toolkit'
import type { PayloadAction } from '@reduxjs/toolkit'

export interface IBaseUrlState {
  current: string;
}

const initialState: IBaseUrlState = {
  current: 'http://localhost:5008',
}

export const baseUrlSlice = createSlice({
  name: 'baseUrl',
  initialState,
  reducers: {
    setBaseUrl: (state, action: PayloadAction<string>) => {
      state.current = action.payload;
    },
  },
})

export const { setBaseUrl } = baseUrlSlice.actions

export default baseUrlSlice.reducer
