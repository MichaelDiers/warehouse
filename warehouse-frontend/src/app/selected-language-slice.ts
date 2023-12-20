import { createSlice } from '@reduxjs/toolkit'
import type { PayloadAction } from '@reduxjs/toolkit'
import Language from '../types/language.enum'

export interface ISelectedLanguageState {
  current: Language
}

const initialState: ISelectedLanguageState = {
  current: Language.DE
}

export const selectedLanguageSlice = createSlice({
  name: 'selectedLanguage',
  initialState,
  reducers: {
    setSelectedLanguage: (state, action: PayloadAction<Language>) => {
      state.current = action.payload;
    },
  },
})

export const { setSelectedLanguage } = selectedLanguageSlice.actions

export default selectedLanguageSlice.reducer
