import Form from "../../components/form-elements/generic/Form";
import Submit from '../../components/form-elements/generic/Submit';
import Password from "../../components/form-elements/user/Password";
import UserName from "../../components/form-elements/user/UserName";
import IText from "../../text/text";
import AppRoutes from "../../types/app-routes.enum";
import { Link } from "react-router-dom";

export function SignInForm({
  disabled,
  id,
  idError,
  onSubmit,
  password,
  passwordError,
  setId,
  setIdError,
  setPassword,
  setPasswordError,
  text
}: {
  disabled: boolean,
  id: string,
  idError: string,
  onSubmit: () => void,
  password: string,
  passwordError: string,
  setId: (id: string) => void,
  setIdError: (error: string) => void,
  setPassword: (password: string) => void,
  setPasswordError: (error: string) => void,
  text: IText
}) {
  return (
    <>
      <Form onSubmit={onSubmit}>
        <UserName
          error={idError}
          setError={setIdError}
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
        <Submit
          disabled={disabled}
          id='signInSubmit'
          label={text.genericSignInLabel}
        />
      </Form>
      <Link to={AppRoutes.SIGN_UP}>{text.genericSignUpLabel}</Link>
    </>
  )
}
