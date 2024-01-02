import Form from "../../components/form-elements/generic/Form";
import Submit from '../../components/form-elements/generic/Submit';
import Password from "../../components/form-elements/user/Password";
import UserName from "../../components/form-elements/user/UserName";
import IText from "../../text/text";

export function SignInForm({
  disabled,
  error,
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
  error?: string,
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
    <Form
      error={error}
      header={text.signInHeadline}
      id='sign-in-form'
      onSubmit={onSubmit}>
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
  )
}
