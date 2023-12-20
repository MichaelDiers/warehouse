import { BaseQueryApi } from '@reduxjs/toolkit/dist/query';
import IText from './text';
import { RootState } from '../app/store';
import Language from '../types/language.enum';
import deText from './de-text';
import ApplicationError from '../types/application-error';

const selectText = (api: BaseQueryApi) : IText => {
  const language = (api.getState() as RootState).selectedLanguage.current;
  switch (language) {
    case Language.DE:
      return deText;
    default:
      throw new ApplicationError(`Unknown language ${language}.`);
  }
}

export default selectText;
