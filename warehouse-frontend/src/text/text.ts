interface IText {
  genericBackLabel: string;
  genericSignInLabel: string;
  genericSignUpLabel: string;
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
  stockItemDeleteDeleteSubmitLabel: string;
  stockItemDetailsHeader: string;
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
  stockItemUpdateHeader: string;
  stockItemUpdateLinkLabel: string;
  stockItemUpdateSubmitLabel: string;
  userNameLabel: string;
  userNameTooLongError: (length: number) => string;
  userNameTooShortError: (length: number) => string;

  optionsError: string;
  cannotFindUrlForUrnError: (urn: string) => string;
  cannotFindUrlError: (url: string) => string;
  signIn400: string;
  signIn401: string;
  signIn403: string;
  signIn404: string;
  signIn500_1: string;
  signIn500_2: string;
  signIn500_3: string;

  signUp400: string;
  signUp401: string;
  signUp403: string;
  signUp409: string;
  signUp500_1: string;
  signUp500_2: string;
  signUp500_3: string;

  stockItemListPending: string;
  stockItemListRejected: string;

  stockItemUnknownTypeError: (unknownType: string) => string;

  stockItemDelete401: string;
  stockItemDelete403: string;
  stockItemDelete404: string;
  stockItemDelete500_1: string;
  stockItemDelete500_2: string;
  stockItemDelete500_3: string;

  stockItemUpdate400: string;
  stockItemUpdate401: string;
  stockItemUpdate403: string;
  stockItemUpdate404: string;
  stockItemUpdate409: string;
  stockItemUpdate500_1: string;
  stockItemUpdate500_2: string;
  stockItemUpdate500_3: string;
}


export default IText;
