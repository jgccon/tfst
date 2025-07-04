// src/pages/index.ts
import type { APIRoute } from 'astro';

export const GET: APIRoute = () => {
  return new Response(null, {
    status: 302,
    headers: {
      Location: '/en/',
    },
  });
};
