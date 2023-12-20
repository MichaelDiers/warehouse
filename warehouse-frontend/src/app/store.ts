import { configureStore, ThunkAction, Action } from '@reduxjs/toolkit';
import apiSlice from './api-slice';
import { userSlice } from './user-slice';
import optionsReducer from './options-slice';
import selectedStockItemReducer from './selected-stock-item-slice';

export const store = configureStore({
  reducer: {
    api: apiSlice.reducer,
    user: userSlice.reducer,
    options: optionsReducer,
    selectedStockItem: selectedStockItemReducer
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
