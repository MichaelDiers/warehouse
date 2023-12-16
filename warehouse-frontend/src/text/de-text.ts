import IText from "./text";

const deText: IText = {
  displayName: 'Display Name',
  displayNameTooLongError: (length: number) => `Display name hat mehr als ${length} Zeichen`,
  displayNameTooShortError: (length: number) => `Display name hat weniger als ${length} Zeichen`,
  forbiddenPasswords: ['password', '12345678', '01234567', '76543210'],
  forbiddenPasswordsError: 'Passwort ist zu einfach',
  invitationCodeLabel: 'Invitation Code',
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
