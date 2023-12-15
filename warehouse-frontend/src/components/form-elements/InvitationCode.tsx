import IText from "../../text/text";
import FormElement from "./FormElement";

const InvitationCode = ({
    id = 'invitationCode',
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
            label={text.invitationCodeLabel}
            setValue={setValue}
            type="text"
            value={value}
        />
    )
}

export default InvitationCode;
