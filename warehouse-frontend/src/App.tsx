import { useState } from 'react';
import { HashRouter, Routes, Route } from 'react-router-dom'
import { SignIn } from './features/sign-in/SignIn';
import { SignUp } from './features/sign-up/SignUp';
import deText from './text/de-text';
import RequiresUser from './features/requires-token/RequiresUser';
import { ShoppingList } from './features/shopping-list/ShoppingList';
import AppRoutes from './types/app-routes.enum';
import { Sign } from 'crypto';

function App() {
  const [accessToken, setAccessToken] = useState('');
  const [refreshToken, setRefreshToken] = useState('');
  const [text, setText] = useState(deText);

  return (
    <div className="App">
      <HashRouter>
        <Routes>
          <Route index element={<RequiresUser roles={['User']}>{<ShoppingList text={text}/>}</RequiresUser>}/>
          <Route path={AppRoutes.SHOPPING_LIST} element={<RequiresUser roles={[]}>{<ShoppingList text={text}/>}</RequiresUser>}/>
          <Route path={AppRoutes.SIGN_IN} element={<SignIn setAccessToken={setAccessToken} setRefreshToken={setRefreshToken} text={text}/>}/>
          <Route path={AppRoutes.SIGN_UP} element={<SignUp setAccessToken={setAccessToken} setRefreshToken={setRefreshToken} text={text}/>}/>
        </Routes>
      </HashRouter>
    </div>
  );
}

export default App;
