import { configureStore, ThunkAction, Action } from '@reduxjs/toolkit';
import apiSlice from './api-slice';
import { userSlice } from './user-slice';
import optionsReducer from './options-slice';
import selectedStockItemReducer from './selected-stock-item-slice';
import selectedLanguageReducer from './selected-language-slice';
import apiKeyReducer from './api-key-slice';
import baseUrlReducer from './base-url-slice';

export const store = configureStore({
  reducer: {
    api: apiSlice.reducer,
    user: userSlice.reducer,
    options: optionsReducer,
    selectedStockItem: selectedStockItemReducer,
    selectedLanguage: selectedLanguageReducer,
    apiKey: apiKeyReducer,
    baseUrl: baseUrlReducer,
  },
  middleware: (getDefaultMiddleware) =>
    getDefaultMiddleware().concat(apiSlice.middleware),
});

export type AppDispatch = typeof store.dispatch;
export type RootState = ReturnType<typeof store.getState>;
export type AppThunk<ReturnType = void> = ThunkAction<
  ReturnType,
  RootState,
  unknown,
  Action<string>
>;
