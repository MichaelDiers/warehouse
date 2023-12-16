import { useState } from "react";
import Form from "../../components/form-elements/Form";
import Password from "../../components/form-elements/Password";
import SignInSubmit from "../../components/form-elements/SignInSubmit";
import UserName from "../../components/form-elements/UserName";
import IText from "../../text/text";
import { ISignInRequest, useSignInMutation } from "./sign-in-api-slice";
import { QueryStatus } from "@reduxjs/toolkit/dist/query";
import AppRoutes from "../../types/app-routes.enum";
import { Link, Navigate } from "react-router-dom";
import { useAppDispatch, useAppSelector } from '../../app/hooks';
import { selectUser } from '../../app/selectors';
import { updateUserThunk } from '../../app/user-slice';

export function SignIn({
  text
}: {
  text: IText
}) {
  const [signIn, { status }] = useSignInMutation();

  const [password, setPassword] = useState('password');
  const [passwordError, setPasswordError] = useState('');
  const [id, setId] = useState('userName');
  const [error, setError] = useState('');
  const disabled = status === QueryStatus.pending
    || !password
    || !id
    || passwordError !== '';

  const user = useAppSelector(selectUser);

  const dispatch = useAppDispatch();

  const onSubmit = () => {
    setError('');

    const args: ISignInRequest = {
      password,
      id
    };

    signIn(args)
      .unwrap()
      .then((result) => {
        const { accessToken, refreshToken } = result;
        if (!accessToken || !refreshToken) {
          setError('Unable to sign in. Internal error.');
        }
        else {
          dispatch(updateUserThunk(accessToken, refreshToken));
        }
      }).catch((err): void => {
        const { status } = err;
        if (status) {
          switch (status) {
            case 401:
              setError('Invalid invitation code');
              break;
            default:
              setError(JSON.stringify(err));
              break;
          }
        }
      });
  };

  if (user) {
    return (<Navigate to={AppRoutes.SHOPPING_LIST} />)
  }

  return (
    <>
      <Form onSubmit={onSubmit}>
        <div></div>
        <label>ERROR</label>
        <div>{error}</div>
        <div></div>
        <label>status</label>
        <div>{status}</div>

        <UserName
          setValue={setId}
          text={text}
          value={id}
        />
        <Password
          error={passwordError}
          setError={setPasswordError}
          setValue={setPassword}
          text={text}
          value={password}
        />
        <SignInSubmit
          disabled={disabled}
          text={text} />
      </Form>
      <Link to={AppRoutes.SIGN_UP}>
        sign up
      </Link>
    </>
  )
}
