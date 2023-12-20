import { createSlice } from '@reduxjs/toolkit'
import type { PayloadAction } from '@reduxjs/toolkit'
import ILink from '../types/link';

export interface OptionsState {
  links: ILink[];
}

const initialState: OptionsState = {
  links: []
}

export const optionsSlice = createSlice({
  name: 'options',
  initialState,
  reducers: {
    append: (state, action: PayloadAction<ILink[]>) => {
      state.links = state.links.concat(action.payload);
    },
    reset: (state) => {
      state.links = [];
    },
  },
})

export const { append, reset } = optionsSlice.actions

export default optionsSlice.reducer