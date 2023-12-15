import IText from "../../text/text";

export function ShoppingList({
    text
} : {
    text: IText
}) {
    return (
        <h1>{text.shoppingListHeader}</h1>
    )
}
