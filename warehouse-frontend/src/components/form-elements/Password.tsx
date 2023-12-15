import IText from "../../text/text";
import FormElement from "./FormElement";

const Password = ({
    id = 'password',
    label,
    setValue,
    text,
    value
} : {
    id?: string,
    label?: string,
    setValue: (value: string) => void,
    text: IText,
    value: string
}) => {
    return (
        <FormElement
            id={id}
            label={label || text.passwordLabel}
            setValue={setValue}
            type="text"
            value={value}
        />
    )
}

export default Password;
