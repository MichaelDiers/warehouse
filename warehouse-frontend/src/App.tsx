import { useState } from 'react';
import { HashRouter, Routes, Route } from 'react-router-dom'
import { SignIn } from './features/sign-in/SignIn';
import { SignUp } from './features/sign-up/SignUp';
import deText from './text/de-text';
import RequiresUser from './features/requires-token/RequiresUser';
import { StockItemList } from './features/stock-item-list/StockItemList';
import AppRoutes from './types/app-routes.enum';
import { StockItemCreate } from './features/stock-item-create/StockItemCreate';

function App() {
  const [text, setText] = useState(deText);

  return (
    <div className="App">
      <HashRouter>
        <Routes>
          <Route index element={<RequiresUser roles={['User']}>{<StockItemList text={text} />}</RequiresUser>} />
          <Route path={AppRoutes.STOCK_ITEM_LIST} element={<RequiresUser roles={[]}>{<StockItemList text={text} />}</RequiresUser>} />
          <Route path={AppRoutes.SIGN_IN} element={<SignIn text={text} />} />
          <Route path={AppRoutes.SIGN_UP} element={<SignUp text={text} />} />
          <Route path={AppRoutes.STOCK_ITEM_CREATE} element={<RequiresUser roles={[]}>{<StockItemCreate text={text} />}</RequiresUser>} />
        </Routes>
      </HashRouter>
    </div>
  );
}

export default App;
