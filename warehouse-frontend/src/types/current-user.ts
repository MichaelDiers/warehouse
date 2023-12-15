export default interface ICurrentUser {
    accessToken: string;
    refreshToken: string;
    roles: [string];
    userId: string;
    notBefore: string;
    expires: string;
}
