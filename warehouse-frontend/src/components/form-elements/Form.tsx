const Form = ({
    children,
    onSubmit
} : {
    children: string | JSX.Element | JSX.Element[],
    onSubmit: () => void
}): JSX.Element =>  {
    const onFormSubmit = (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        onSubmit();
    };

    return (
        <form onSubmit={onFormSubmit}>
            {children}
        </form>
    )
}

export default Form;
