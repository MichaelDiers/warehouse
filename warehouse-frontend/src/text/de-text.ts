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
  stockItemDeleteDeleteSubmitLabel: 'Löschen',
  stockItemDetailsHeader: 'Stock Item Details',
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
  stockItemUpdateHeader: 'Update Stock Item',
  stockItemUpdateLinkLabel: 'Aktualisieren',
  stockItemUpdateSubmitLabel: 'Update',
  userNameLabel: 'Benutzername',
  userNameTooLongError: (length: number) => `Benutzername hat mehr als ${length} Zeichen`,
  userNameTooShortError: (length: number) => `Benutzername hat weniger als ${length} Zeichen`,

  optionsError: 'Service nicht verfügbar. Bitte versuchen sie später noch einmal. [1]',
  cannotFindUrlForUrnError: (urn: string) => `Service nicht verfügbar. Bitte versuchen sie später noch einmal. [${urn}]`,
  cannotFindUrlError: (url: string) => `Service nicht verfügbar. Bitte versuchen sie später noch einmal. [${url}]`,
  
  signIn400: 'Ungültiger Benutzer oder Passwort. [2]',
  signIn401: 'Unbekannte Kombination aus Benutzername und Passwort.',
  signIn403: 'Service nicht verfügbar. Bitte versuchen sie später noch einmal. [3]',
  signIn404: 'Unbekannte Kombination aus Benutzername und Passwort',
  signIn500_1: 'Service nicht verfügbar. Bitte versuchen sie später noch einmal. [4]',
  signIn500_2: 'Service nicht verfügbar. Bitte versuchen sie später noch einmal. [5]',
  signIn500_3: 'Service nicht verfügbar. Bitte versuchen sie später noch einmal. [6]',

  signUp400: 'Ungültige Eingabedaten [7]',
  signUp401: 'Inivatation Code ist ungültig.',
  signUp403: 'Service nicht verfügbar. Bitte versuchen sie später noch einmal. [8]',
  signUp409: 'Benutzer existiert bereits.',
  signUp500_1: 'Service nicht verfügbar. Bitte versuchen sie später noch einmal. [10]',
  signUp500_2: 'Service nicht verfügbar. Bitte versuchen sie später noch einmal. [11]',
  signUp500_3: 'Service nicht verfügbar. Bitte versuchen sie später noch einmal. [12]',

  stockItemListPending: 'Lade Daten',
  stockItemListRejected: 'Service nicht verfügbar. Bitte versuchen sie später noch einmal. [13]',

  stockItemUnknownTypeError: (unknownType: string): string => `Service nicht verfügbar. Bitte versuchen sie später noch einmal. [14, ${unknownType}]`,

  stockItemDelete401: 'Fehlende Berechtigung zur Löschung des Eintrages.',
  stockItemDelete403: 'Service nicht verfügbar. Bitte versuchen sie später noch einmal. [15]',
  stockItemDelete404: 'Eintrag existiert nicht und kann nicht gelöscht werden.',
  stockItemDelete500_1: 'Service nicht verfügbar. Bitte versuchen sie später noch einmal. [16]',
  stockItemDelete500_2: 'Service nicht verfügbar. Bitte versuchen sie später noch einmal. [17]',
  stockItemDelete500_3: 'Service nicht verfügbar. Bitte versuchen sie später noch einmal. [18]',

  stockItemUpdate400: 'Ungültige Daten gefunden. [19]',
  stockItemUpdate401: 'Fehlende Berechtigung zur Aktualisierung des Eintrages.',
  stockItemUpdate403: 'Service nicht verfügbar. Bitte versuchen sie später noch einmal. [20]',
  stockItemUpdate404: 'Eintrag existiert nicht und kann nicht aktualisiert werden.',
  stockItemUpdate409: 'Eintrag mit den aktualisierten Daten exisitiert bereits.',
  stockItemUpdate500_1: 'Service nicht verfügbar. Bitte versuchen sie später noch einmal. [21]',
  stockItemUpdate500_2: 'Service nicht verfügbar. Bitte versuchen sie später noch einmal. [22]',
  stockItemUpdate500_3: 'Service nicht verfügbar. Bitte versuchen sie später noch einmal. [23]',
}

export default deText;
