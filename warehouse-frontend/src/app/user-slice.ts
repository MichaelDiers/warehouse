import { createSlice, PayloadAction } from '@reduxjs/toolkit'
import ICurrentUser from '../types/current-user';
import { AppDispatch } from './store';

interface IUserState {
  current?: ICurrentUser;
}

const initialState = (): IUserState => ({});

export const userSlice = createSlice({
  name: 'user',
  initialState,
  reducers: {
    resetUser: (state) => {
      state.current = undefined;
    },
    updateUser: (state, action: PayloadAction<ICurrentUser>) => {
      state.current = action.payload;
    }
  },  
});

export function updateUserThunk(accessToken: string, refreshToken: string) {
  return (dispatch: AppDispatch) => {
    if (!accessToken || !refreshToken) {
      return;
    }

    const splittedToken = accessToken.split('.');
    if (splittedToken.length !== 3) {
      return;
    }

    const payload: any = JSON.parse(window.atob(splittedToken[1]));

    const user: ICurrentUser = {
      accessToken,
      expires: payload['exp'],
      notBefore: payload['nbf'],
      refreshToken,
      roles: payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'],
      userId: payload['UserId']
    };

    dispatch(userSlice.actions.updateUser(user));
  }
}
