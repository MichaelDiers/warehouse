import IText from "../../text/text";
import Submit from "./Submit";

const SignInSubmit = ({
    disabled,
    id = 'submit',
    text,
} : {
    disabled?: boolean,
    id?: string,
    text: IText
}) => {
    return (
        <Submit
            disabled={disabled}
            id={id}
            label={text.signInSubmit}
        />
    )
}

export default SignInSubmit;
