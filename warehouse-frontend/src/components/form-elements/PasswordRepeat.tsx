import IText from "../../text/text";
import Password from "./Password";

const PasswordRepeat = ({
    id = 'passwordRepeat',
    setValue,
    text,
    value
} : {
    id?: string,
    setValue: (value: string) => void,
    text: IText,
    value: string
}) => {
    return (
        <Password
            id={id}
            label={text.passwordRepeatLabel}
            setValue={setValue}
            text={text}
            value={value}
        />
    )
}

export default PasswordRepeat;
