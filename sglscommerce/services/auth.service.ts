import { ApiError, api } from "@/services/api";

type ClientLoginCredentials = {
  email: string;
  senha: string;
};

type ClientLoginResponse = {
  token: string;
  expiracao: string;
  tipoAcesso: string;
};

const CLIENT_TOKEN_KEY = "sgls-commerce-client-token";
const CLIENT_TOKEN_EXPIRATION_KEY = "sgls-commerce-client-token-expiration";

export async function loginClient(
  credentials: ClientLoginCredentials,
): Promise<ClientLoginResponse> {
  const { data } = await api.post<ClientLoginResponse>(
    "/v1/auth/cliente/login",
    credentials,
  );

  sessionStorage.setItem(CLIENT_TOKEN_KEY, data.token);
  sessionStorage.setItem(CLIENT_TOKEN_EXPIRATION_KEY, data.expiracao);

  return data;
}

export function getClientLoginErrorMessage(error: unknown): string {
  if (error instanceof ApiError && error.statusCode === 401) {
    return "E-mail ou senha inválidos.";
  }

  return "Não foi possível entrar agora. Verifique sua conexão e tente novamente.";
}
