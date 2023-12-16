interface IText {
  displayName: string;
  displayNameTooLongError: (length: number) => string;
  displayNameTooShortError: (length: number) => string;
  forbiddenPasswords: string[];
  forbiddenPasswordsError: string;
  invitationCodeLabel: string;
  passwordLabel: string;
  passwordTooLongError: (length: number) => string;
  passwordTooShortError: (length: number) => string;
  passwordRepeatLabel: string;
  passwordMismatchError: string;
  shoppingListHeader: string;
  signInSubmit: string;
  signUpSubmit: string;
  userNameLabel: string;
  userNameTooLongError: (length: number) => string;
  userNameTooShortError: (length: number) => string;
}

export default IText;
