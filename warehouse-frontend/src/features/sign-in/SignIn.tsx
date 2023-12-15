import { useState } from "react";
import Form from "../../components/form-elements/Form";
import Password from "../../components/form-elements/Password";
import SignInSubmit from "../../components/form-elements/SignInSubmit";
import UserName from "../../components/form-elements/UserName";
import IText from "../../text/text";
import { ISignInRequest, useSignInMutation } from "./sign-in-api-slice";
import { v4 } from 'uuid';
import { QueryStatus } from "@reduxjs/toolkit/dist/query";
import AppRoutes from "../../types/app-routes.enum";
import { Link } from "react-router-dom";

export function SignIn({
    setAccessToken,
    setRefreshToken,
    text
}: {
    setAccessToken: (token: string) => void,
    setRefreshToken: (token: string) => void,
    text: IText
}) {
    const [signIn, { status }] = useSignInMutation();



    const [password, setPassword] = useState(v4());
    const [id, setId] = useState(v4());
    const [error, setError] = useState('');
    const disabled = !(password && id) || status === QueryStatus.pending;

    const onSubmit = () => {
        setError('');

        const args: ISignInRequest = {
            password,
            id
        };

        signIn(args)
            .unwrap()
            .then((result) => {
                const { accessToken, refreshToken } = result;
                if (!accessToken || !refreshToken) {
                    setError('Unable to sign up. Internal error.');
                }
                else {
                    setAccessToken(accessToken);
                    setRefreshToken(refreshToken);
                }
            }).catch((err): void => {
                const { status } = err;
                if (status) {
                    switch (status) {
                        case 401:
                            setError('Invalid invitation code');
                            break;
                        default:
                            setError(JSON.stringify(err));
                            break;
                    }
                }
            });
    };

    return (
        <>
            <Form onSubmit={onSubmit}>
                <label>ERROR</label>
                <div>{error}</div>
                <label>status</label>
                <div>{status}</div>
                <UserName
                    setValue={setId}
                    text={text}
                    value={id}
                />
                <Password
                    setValue={setPassword}
                    text={text}
                    value={password}
                />
                <SignInSubmit
                    disabled={disabled}
                    text={text} />
            </Form>
            <Link to={AppRoutes.SIGN_UP}>
                sign up
            </Link>
        </>
    )
}
