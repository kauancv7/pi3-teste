"use client";
import { useRouter } from "next/navigation";

import { type FormEvent, useState } from "react";

import {
  getClientLoginErrorMessage,
  loginClient,
} from "@/services/auth.service";

type LoginFormProps = {
  onSuccess?: () => void;
};

export function LoginForm({ onSuccess }: LoginFormProps) {
  const [email, setEmail] = useState("");
  const [senha, setSenha] = useState("");
  const [showPassword, setShowPassword] = useState(false);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");
  const router = useRouter();

  async function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();
    setError("");
    setSuccess("");
    setIsSubmitting(true);

    try {
      await loginClient({ email, senha });
      setSuccess("Login realizado com sucesso.");
      onSuccess?.();
      
      // Redireciona para a tela inicial
      router.push("/");
      
      // Atualiza a página para garantir que componentes do servidor 
      // (como o Header) percebam que o usuário está logado
      router.refresh(); 
      
    } catch (loginError) {
      setError(getClientLoginErrorMessage(loginError));
    } finally {
      setIsSubmitting(false);
    }
  }

  return (
    <form className="space-y-5" onSubmit={handleSubmit} noValidate>
      <div>
        <label htmlFor="login-email" className="mb-2 block text-xs font-bold text-[#231c19]">
          E-mail <span className="text-[#8b4513]">*</span>
        </label>
        <input
          id="login-email"
          type="email"
          required
          autoComplete="email"
          value={email}
          onChange={(event) => setEmail(event.target.value)}
          className="h-11 w-full border border-[#d8d3cc] bg-white px-3 text-sm text-[#231c19] outline-none transition-colors focus:border-[#2d221e]"
        />
      </div>

      <div>
        <label htmlFor="login-password" className="mb-2 block text-xs font-bold text-[#231c19]">
          Senha <span className="text-[#8b4513]">*</span>
        </label>
        <div className="relative">
          <input
            id="login-password"
            type={showPassword ? "text" : "password"}
            required
            minLength={6}
            autoComplete="current-password"
            value={senha}
            onChange={(event) => setSenha(event.target.value)}
            className="h-11 w-full border border-[#d8d3cc] bg-white px-3 pr-11 text-sm text-[#231c19] outline-none transition-colors focus:border-[#2d221e]"
          />
          <button
            type="button"
            onClick={() => setShowPassword((value) => !value)}
            className="absolute right-3 top-1/2 -translate-y-1/2 text-xs font-bold text-[#7c726b] hover:text-[#231c19]"
            aria-label={showPassword ? "Ocultar senha" : "Mostrar senha"}
          >
            {showPassword ? "Ocultar" : "Mostrar"}
          </button>
        </div>
      </div>

      {error && <p role="alert" className="text-sm text-[#b93826]">{error}</p>}
      {success && <p role="status" className="text-sm text-[#68754b]">{success}</p>}

      <button
        type="submit"
        disabled={isSubmitting}
        className="h-11 w-full bg-[#2d221e] text-xs font-black uppercase tracking-wider !text-white transition-colors hover:bg-[#42322c] disabled:cursor-not-allowed disabled:opacity-60"
      >
        {isSubmitting ? "Entrando..." : "Entrar"}
      </button>
    </form>
  );
}