import React from "react";

const Submit = ({
    autoFocus,
    disabled,
    height,
    id,
    label,
    type = 'submit',
    width
} : {
    autoFocus?: boolean,
    disabled?: boolean,
    height?: number,
    id: string,
    label: string,
    type?: 'submit',
    width?: number
}) => {
    return (
        <>
            <input
                autoFocus={autoFocus}
                disabled={disabled}
                height={height}
                id={id}
                type={type}
                value={label}
                width={width}
            />
        </>
    )
}

export default Submit;