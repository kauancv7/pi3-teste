import { z } from "@/lib/form";

export const signInSchema = z.object({
  email: z.string().email("Informe um e-mail válido."),
  password: z.string().min(1, "Informe sua senha."),
});

export type SignInFormData = z.infer<typeof signInSchema>;
