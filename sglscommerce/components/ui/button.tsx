import { type ButtonHTMLAttributes } from "react";

import { cn } from "@/lib/cn";

type ButtonVariant = "primary" | "secondary" | "action" | "ghost" | "destructive" | "outline";
type ButtonSize = "sm" | "md" | "lg";

type ButtonProps = ButtonHTMLAttributes<HTMLButtonElement> & {
  size?: ButtonSize;
  variant?: ButtonVariant;
};

const variantClasses: Record<ButtonVariant, string> = {
  primary: "bg-[#2d221e] text-[#f8f9fa] hover:bg-[#42322c] focus-visible:ring-[#2d221e]",
  action: "bg-[#8b4513] text-white hover:bg-[#a0522d] focus-visible:ring-[#8b4513]",
  secondary: "bg-[#eeeae4] text-[#2d221e] hover:bg-[#e2ddd5] focus-visible:ring-[#2d221e]",
  outline: "border border-[#2d221e] bg-transparent text-[#2d221e] hover:bg-[#2d221e] hover:text-[#f8f9fa] focus-visible:ring-[#2d221e]",
  ghost: "text-[#2d221e] hover:bg-[#eeeae4] focus-visible:ring-[#2d221e]",
  destructive: "bg-[#b93826] text-white hover:bg-[#9c2f1f] focus-visible:ring-[#b93826]",
};

const sizeClasses: Record<ButtonSize, string> = {
  sm: "h-9 px-4 text-xs font-bold uppercase tracking-wider",
  md: "h-11 px-5 text-xs font-bold uppercase tracking-wider",
  lg: "h-12 px-6 text-sm font-bold uppercase tracking-wider",
};

export function Button({ className, size = "md", variant = "primary", ...props }: ButtonProps) {
  return (
    <button
      className={cn(
        "inline-flex items-center justify-center gap-2 rounded-none font-bold uppercase tracking-wider transition-colors duration-200 focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-offset-2 disabled:cursor-not-allowed disabled:opacity-60",
        variantClasses[variant],
        sizeClasses[size],
        className,
      )}
      {...props}
    />
  );
}
