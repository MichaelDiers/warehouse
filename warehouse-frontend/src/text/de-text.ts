import IText from "./text";

const deText: IText = {
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
  shoppingListHeader: 'Shopping List',
  signInSubmit: 'Anmelden',
  signUpSubmit: 'Registrieren',
  userNameLabel: 'Benutzername',
  userNameTooLongError: (length: number) => `Benutzername hat mehr als ${length} Zeichen`,
  userNameTooShortError: (length: number) => `Benutzername hat weniger als ${length} Zeichen`,
}

export default deText;
