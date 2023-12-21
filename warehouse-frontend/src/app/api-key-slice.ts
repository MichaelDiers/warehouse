import { createSlice } from '@reduxjs/toolkit'
import type { PayloadAction } from '@reduxjs/toolkit'

export interface IApiKeyState {
  current: string;
}

const initialState: IApiKeyState = {
  current: 'The api key',
}

export const apiKeySlice = createSlice({
  name: 'apiKey',
  initialState,
  reducers: {
    setApiKey: (state, action: PayloadAction<string>) => {
      state.current = action.payload;
    },
  },
})

export const { setApiKey } = apiKeySlice.actions

export default apiKeySlice.reducer
