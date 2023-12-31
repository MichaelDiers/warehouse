import Form from "../../components/form-elements/generic/Form";
import Password from "../../components/form-elements/user/Password";
import UserName from "../../components/form-elements/user/UserName";
import IText from "../../text/text";
import PasswordRepeat from "../../components/form-elements/user/PasswordRepeat";
import InvitationCode from "../../components/form-elements/user/InvitationCode";
import DisplayName from "../../components/form-elements/user/DisplayName";
import AppRoutes from "../../types/app-routes.enum";
import { Link } from "react-router-dom";
import Submit from '../../components/form-elements/generic/Submit';

export function SignUpForm({
  disabled,
  displayName,
  displayNameError,
  error,
  id,
  idError,
  invitationCode,
  invitationCodeError,
  onSubmit,
  password,
  passwordError,
  passwordRepeat,
  passwordRepeatError,
  setDisplayName,
  setDisplayNameError,
  setId,
  setIdError,
  setInvitationCode,
  setInvitationCodeError,
  setPassword,
  setPasswordError,
  setPasswordRepeat,
  setPasswordRepeatError,
  validatePassword,
  validatePasswordRepeat,
  text
}: {
  disabled: boolean,
  displayName: string,
  displayNameError: string,
  error?: string,
  id: string,
  idError: string,
  invitationCode: string,
  invitationCodeError: string,
  onSubmit: () => void,
  password: string,
  passwordError: string,
  passwordRepeat: string,
  passwordRepeatError: string,
  setDisplayName: (displayName: string) => void,
  setDisplayNameError: (error: string) => void,
  setId: (id: string) => void,
  setIdError: (error: string) => void,
  setInvitationCode: (invitationCode: string) => void,
  setInvitationCodeError: (error: string) => void,
  setPassword: (password: string) => void,
  setPasswordError: (error: string) => void,
  setPasswordRepeat: (error: string) => void,
  setPasswordRepeatError: (error: string) => void,
  validatePassword: (value: string) => string,
  validatePasswordRepeat: (value: string) => string,
  text: IText
}) {
  return (
    <>
      <Form
        error={error}
        header={text.signUpHeadline}
        id='sign-up-form'
        onSubmit={onSubmit}>
        <UserName
          error={idError}
          setError={setIdError}
          setValue={setId}
          text={text}
          value={id}
        />
        <DisplayName
          error={displayNameError}
          setError={setDisplayNameError}
          setValue={setDisplayName}
          text={text}
          value={displayName}
        />
        <Password
          additionalValidation={validatePassword}
          error={passwordError}
          setError={setPasswordError}
          setValue={setPassword}
          text={text}
          value={password}
        />
        <PasswordRepeat
          additionalValidation={validatePasswordRepeat}
          error={passwordRepeatError}
          setError={setPasswordRepeatError}
          setValue={setPasswordRepeat}
          text={text}
          value={passwordRepeat}
        />
        <InvitationCode
          error={invitationCodeError}
          setError={setInvitationCodeError}
          setValue={setInvitationCode}
          text={text}
          value={invitationCode}
        />
        <Submit
          disabled={disabled}
          id='signUpSubmit'
          label={text.genericSignUpLabel}
        />
      </Form>
      <Link to={AppRoutes.SIGN_IN}>{text.genericSignInLabel}</Link>
    </>
  )
}
