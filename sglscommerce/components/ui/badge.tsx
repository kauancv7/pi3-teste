import { type ComponentPropsWithoutRef } from "react";

import { cn } from "@/lib/cn";

type BadgeVariant = "primary" | "secondary" | "success" | "warning" | "destructive" | "discount";

type BadgeProps = ComponentPropsWithoutRef<"span"> & {
  variant?: BadgeVariant;
};

const variantClasses: Record<BadgeVariant, string> = {
  primary: "bg-[#2d221e] text-[#f8f9fa]",
  discount: "bg-[#8b4513] text-[#ffffff]",
  secondary: "bg-[#eeeae4] text-[#2d221e] border border-[#e2ddd5]",
  success: "bg-[#5c6f4e] text-white",
  warning: "bg-[#c68b59] text-white",
  destructive: "bg-[#b93826] text-white",
};

export function Badge({ className, variant = "secondary", ...props }: BadgeProps) {
  return (
    <span
      className={cn(
        "inline-flex items-center rounded-none px-2 py-0.5 text-[10px] font-black uppercase tracking-wider leading-none",
        variantClasses[variant],
        className,
      )}
      {...props}
    />
  );
}
