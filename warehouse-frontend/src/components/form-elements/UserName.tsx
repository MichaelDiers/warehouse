import IText from "../../text/text";
import FormElement from "./FormElement";

const UserName = ({
    id = 'userName',
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
        <FormElement
            id={id}
            label={text.userNameLabel}
            setValue={setValue}
            type="text"
            value={value}
        />
    )
}

export default UserName;
