export function InProgressIndicator({
  children,
  isInProgress
}:{
  children: JSX.Element,
  isInProgress?: boolean
}) {
  const indicator = (
    <>
    <div className='process-indicator overlay'/>
    <div className='process-indicator loader'/>
    </>
  );

  return (
    <div>
      {
        isInProgress 
        ? indicator
        : <></>
      }
      
      {children}
    </div>
  )
}
