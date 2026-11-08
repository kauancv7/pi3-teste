import { type ComponentPropsWithoutRef, type ElementType, type ReactNode } from "react";

import { cn } from "@/lib/cn";

type TypographyVariant = "display" | "h1" | "h2" | "h3" | "body" | "small" | "label";

type TypographyProps<T extends ElementType> = {
  as?: T;
  children: ReactNode;
  variant?: TypographyVariant;
} & Omit<ComponentPropsWithoutRef<T>, "as" | "children">;

const defaultElement: Record<TypographyVariant, ElementType> = {
  display: "h1",
  h1: "h1",
  h2: "h2",
  h3: "h3",
  body: "p",
  small: "p",
  label: "span",
};

const variantClasses: Record<TypographyVariant, string> = {
  display: "text-4xl font-bold tracking-tight text-foreground sm:text-5xl lg:text-6xl",
  h1: "text-3xl font-bold tracking-tight text-foreground sm:text-4xl",
  h2: "text-2xl font-semibold tracking-tight text-foreground sm:text-3xl",
  h3: "text-xl font-semibold tracking-tight text-foreground sm:text-2xl",
  body: "text-base leading-7 text-muted-foreground",
  small: "text-sm leading-6 text-muted-foreground",
  label: "text-sm font-medium leading-none text-foreground",
};

export function Typography<T extends ElementType = "p">({
  as,
  children,
  className,
  variant = "body",
  ...props
}: TypographyProps<T>) {
  const Component = as ?? defaultElement[variant];

  return (
    <Component className={cn(variantClasses[variant], className)} {...props}>
      {children}
    </Component>
  );
}
