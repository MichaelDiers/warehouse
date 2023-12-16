interface IText {
  displayName: string;
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
}

export default IText;
