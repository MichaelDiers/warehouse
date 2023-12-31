const enum Validation {
  DISPLAY_NAME_MAX_LENGTH = 100,
  DISPLAY_NAME_MIN_LENGTH = 8,
  INVITATION_CODE_MAX_LENGTH = 36,
  INVITATION_CODE_MIN_LENGTH = 36,
  PASSWORD_MAX_LENGTH = 100,
  PASSWORD_MIN_LENGTH = 8,
  STOCK_ITEM_NAME_MAX_LENGTH = 100,
  STOCK_ITEM_NAME_MIN_LENGTH = 2,
  STOCK_ITEM_MINIMUM_QUANTITY_MAX = 9999,
  STOCK_ITEM_MINIMUM_QUANTITY_MIN = 0,
  STOCK_ITEM_QUANTITY_MAX = 9999,
  STOCK_ITEM_QUANTITY_MIN = 0,
  USERNAME_MAX_LENGTH = 100,
  USERNAME_MIN_LENGTH = 2,
};

export default Validation;
