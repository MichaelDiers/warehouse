import IText from "../../text/text";
import FormElement from "./FormElement";

const DisplayName = ({
    id = 'displayName',
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
            label={text.displayName}
            setValue={setValue}
            type="text"
            value={value}
        />
    )
}

export default DisplayName;
