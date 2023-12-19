interface IText {
  backLabel: string;
  displayName: string;
  displayNameTooLongError: (length: number) => string;
  displayNameTooShortError: (length: number) => string;
  forbiddenPasswords: string[];
  forbiddenPasswordsError: string;
  invitationCodeFormatError: string;
  invitationCodeLabel: string;
  invitationCodeTooLongError: (length: number) => string;
  invitationCodeTooShortError: (length: number) => string;
  passwordLabel: string;
  passwordTooLongError: (length: number) => string;
  passwordTooShortError: (length: number) => string;
  passwordRepeatLabel: string;
  passwordMismatchError: string;
  stockItemCreateHeader: string;
  stockItemCreateLinkLabel: string;
  stockItemCreateSubmitLabel: string;
  stockItemCreateSubmitAndNewLabel: string;
  stockItemNameLabel: string;
  stockItemMinimumQuantityLabel: string;
  stockItemMinimumQuantityTooLarge: (maximum: number) => string;
  stockItemMinimumQuantityTooSmall: (minimum: number) => string;
  stockItemNameTooLongError: (length: number) => string;
  stockItemNameTooShortError: (length: number) => string;
  stockItemQuantityLabel: string;
  stockItemQuantityTooLarge: (maximum: number) => string;
  stockItemQuantityTooSmall: (minimum: number) => string;
  stockItemListHeader: string;
  signInSubmit: string;
  signUpSubmit: string;
  userNameLabel: string;
  userNameTooLongError: (length: number) => string;
  userNameTooShortError: (length: number) => string;
}

export default IText;
