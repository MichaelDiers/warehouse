import deText from '../text/de-text';
import ApplicationError from '../types/application-error';
import Language from '../types/language.enum';
import { RootState } from './store';

export const selectUser = (state: RootState) => state.user.current;

export const selectText = (state: RootState) => {
  const language = state.selectedLanguage.current;
  switch (language) {
    case Language.DE:
      return deText;
    default:
      throw new ApplicationError(`Unknown language ${language}.`);
  }
}