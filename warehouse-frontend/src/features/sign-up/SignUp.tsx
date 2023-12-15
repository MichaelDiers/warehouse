import { useState } from "react";
import Form from "../../components/form-elements/Form";
import Password from "../../components/form-elements/Password";
import SignUpSubmit from "../../components/form-elements/SignUpSubmit";
import UserName from "../../components/form-elements/UserName";
import IText from "../../text/text";
import PasswordRepeat from "../../components/form-elements/PasswordRepeat";
import InvitationCode from "../../components/form-elements/InvitationCode";
import { ISignUpRequest, useSignUpMutation } from "./sign-up-api-slice";
import DisplayName from "../../components/form-elements/DisplayName";
import { v4 } from 'uuid';
import { QueryStatus } from "@reduxjs/toolkit/dist/query";
import AppRoutes from "../../types/app-routes.enum";
import { Link, Navigate } from "react-router-dom";
import { useAppDispatch, useAppSelector } from "../../app/hooks";
import { selectUser } from "../../app/selectors";
import { updateUserThunk } from "../../app/user-slice";

export function SignUp({
  setAccessToken,
  setRefreshToken,
  text
}: {
  setAccessToken: (token: string) => void,
  setRefreshToken: (token: string) => void,
  text: IText
}) {
  const [signUp, {
    status }] = useSignUpMutation();

  const [displayName, setDisplayName] = useState(v4());
  const [invitationCode, setInvitationCode] = useState('a40b8a6f-4abd-4281-9263-d38265baedcf');
  const [password, setPassword] = useState(v4());
  const [passwordRepeat, setPasswordRepeat] = useState(v4());
  const [id, setId] = useState(v4());
  const [error, setError] = useState('');
  const disabled = !(password && id && passwordRepeat && invitationCode && displayName) || status === QueryStatus.pending;

  const user = useAppSelector(selectUser);
  
  const dispatch = useAppDispatch();
  
  const onSubmit = () => {
    setError('');

    const args: ISignUpRequest = {
      displayName,
      invitationCode,
      password,
      id
    };

    signUp(args)
      .unwrap()
      .then((result) => {
        const { accessToken, refreshToken } = result;
        if (!accessToken || !refreshToken) {
          setError('Unable to sign up. Internal error.');
        }
        else {
          setAccessToken(accessToken);
          setRefreshToken(refreshToken);
          dispatch(updateUserThunk(accessToken, refreshToken));
        }
      }).catch((err): void => {
        const { status } = err;
        if (status) {
          switch (status) {
            case 401:
              setError('Invalid invitation code');
              break;
            case 409:
              setError('User already exists');
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
        <label>ERROR</label>
        <div>{error}</div>
        <label>status</label>
        <div>{status}</div>
        <UserName
          setValue={setId}
          text={text}
          value={id}
        />
        <DisplayName
          setValue={setDisplayName}
          text={text}
          value={displayName}
        />
        <Password
          setValue={setPassword}
          text={text}
          value={password}
        />
        <PasswordRepeat
          setValue={setPasswordRepeat}
          text={text}
          value={passwordRepeat}
        />
        <InvitationCode
          setValue={setInvitationCode}
          text={text}
          value={invitationCode}
        />
        <SignUpSubmit
          disabled={disabled}
          text={text} />
      </Form>
      <Link to={AppRoutes.SIGN_IN}>
        sign in
      </Link>
    </>
  )
}
