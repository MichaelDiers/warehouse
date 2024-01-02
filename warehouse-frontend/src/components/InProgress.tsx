export function InProgressIndicator({
  children,
  className,
  isInProgress
}:{
  children: JSX.Element | JSX.Element[],
  className?: string,
  isInProgress?: boolean
}) {
  const indicator = (
    <>
    <div className='process-indicator overlay'/>
    <div className='process-indicator loader'/>
    </>
  );

  return (
    <div className={className}>
      {
        isInProgress 
        ? indicator
        : <></>
      }
      
      {children}
    </div>
  )
}
