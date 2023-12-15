import { configureStore, ThunkAction, Action } from '@reduxjs/toolkit';
import apiSlice from './api-slice';
import { userSlice } from './user-slice';

export const store = configureStore({
  reducer: {
    api: apiSlice.reducer,
    user: userSlice.reducer,
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
