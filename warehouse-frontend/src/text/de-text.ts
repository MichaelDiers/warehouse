import IText from "./text";

const deText: IText = {
  genericBackLabel: 'Zurück',
  genericSignInLabel: 'Anmelden',
  genericSignUpLabel: 'Registrieren',
  displayName: 'Display Name',
  displayNameTooLongError: (length: number) => `Display name hat mehr als ${length} Zeichen`,
  displayNameTooShortError: (length: number) => `Display name hat weniger als ${length} Zeichen`,
  forbiddenPasswords: ['password', '12345678', '01234567', '76543210'],
  forbiddenPasswordsError: 'Passwort ist zu einfach',
  invitationCodeFormatError: 'Bitte halten Sie sich an das gegebene Format: efdf22de-fbde-4a7f-b864-51858644399c',
  invitationCodeLabel: 'Invitation Code',
  invitationCodeTooLongError: (length: number) => `Invitation Code hat mehr als ${length} Zeichen`,
  invitationCodeTooShortError: (length: number) => `Invitation Code hat weniger als ${length} Zeichen`,
  passwordLabel: 'Passwort',
  passwordRepeatLabel: 'Passwort wiederholen',
  passwordTooLongError: (length: number) => `Passwort hat mehr als ${length} Zeichen`,
  passwordTooShortError: (length: number) => `Passwort hat weniger als ${length} Zeichen`,
  passwordMismatchError: 'Passwörter stimmen nicht überein',
  stockItemCreateHeader: 'Neues Stock Item',
  stockItemCreateLinkLabel: 'Neu',
  stockItemCreateSubmitLabel: 'Anlegen',
  stockItemCreateSubmitAndNewLabel: 'Anlegen und nächstes Anlegen',
  stockItemMinimumQuantityLabel: 'Min. Anzahl',
  stockItemMinimumQuantityTooLarge: (maximum: number) => `Maximalwert von ${maximum} überschritten.`,
  stockItemMinimumQuantityTooSmall: (minimum: number) => `Minimalwert von ${minimum} unterschritten`,
  stockItemNameLabel: 'Name',
  stockItemNameTooLongError: (length: number) => `Name hat mehr als ${length} Zeichen`,
  stockItemNameTooShortError: (length: number) => `Name hat weniger als ${length} Zeichen`,
  stockItemQuantityLabel: 'Anzahl',
  stockItemQuantityTooLarge: (maximum: number) => `Maximalwert von ${maximum} überschritten.`,
  stockItemQuantityTooSmall: (minimum: number) => `Minimalwert von ${minimum} unterschritten`,
  stockItemListHeader: 'Stock Item List',
  userNameLabel: 'Benutzername',
  userNameTooLongError: (length: number) => `Benutzername hat mehr als ${length} Zeichen`,
  userNameTooShortError: (length: number) => `Benutzername hat weniger als ${length} Zeichen`,
}

export default deText;
